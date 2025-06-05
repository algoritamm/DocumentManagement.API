namespace DocumentManagement.Rest.Api.Dto
{
    public class ApiResponseDto
    {
        public List<ItemResponse> ItemResponses { get; set; } = new List<ItemResponse>();
    }
    public class ItemResponse
    {
        public string Item { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
