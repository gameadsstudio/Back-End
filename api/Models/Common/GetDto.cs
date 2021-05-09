namespace api.Models.Common
{
    public class GetDto<T>
    {
        public T Data { get; set; }

        public GetDto(T data)
        {
            Data = data;
        }
    }
}