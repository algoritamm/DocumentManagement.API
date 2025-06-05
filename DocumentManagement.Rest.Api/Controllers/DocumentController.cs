using DocumentManagement.Rest.Api.Dto;
using DocumentManagement.Rest.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DocumentManagement.Rest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _DocumentManagementService;
        private readonly ILogger<DocumentController> _logger;
        public DocumentController(IDocumentService DocumentManagementService, ILogger<DocumentController> logger)
        {
            _DocumentManagementService = DocumentManagementService;
            _logger = logger;
        }

        [HttpPost]
        [Route("DocumentImport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DocumentImport(IEnumerable<DocumentManagementDto> requestData)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(requestData));

            var response = new ApiResponseDto();
            foreach (var item in requestData.OrderBy(x => x.DocumentRefNumber))
            {
                var itemResponse = new ItemResponse { Item = item.DocumentRefNumber };
                try
                {
                    var (isDocumentValid, validationMessage) = _DocumentManagementService.ValidateDocument(item);
                    if (isDocumentValid)
                    {
                        (bool isInserted, string message) = _DocumentManagementService.DocumentImport(item);
                        if (isInserted)
                        {
                            itemResponse.Message = message;
                            itemResponse.IsSuccess = true;
                        }
                        else
                        {
                            itemResponse.Message = message;
                            itemResponse.IsSuccess = false;
                        }
                    }
                    else
                    {
                        itemResponse.Message = validationMessage;
                        itemResponse.IsSuccess = false;

                    }
                }
                catch (Exception ex)
                {
                    itemResponse.IsSuccess = false;
                    itemResponse.Message = $"Error processing document: {ex.Message}";
                }

                response.ItemResponses.Add(itemResponse);
            }
            if (response.ItemResponses.Any(x => x.IsSuccess == true))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
