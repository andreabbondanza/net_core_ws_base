namespace eu.andrewdev.ws_base.models
{
    /**
    * Risposta standard
    */
    public class StandardResponse<T> where T : class
    {
        public T? data { get; set; }
        public string? errorMessage { get; set; } = "";
        public int statusCode { get; set; } = 0;
        public StandardError? error { get; set; }
        public StandardResponse(T? data, string? message = "")
        {
            this.data = data;
            errorMessage = message;

        }
    }
    /**
    * Errore standard
    */
    public class StandardError
    {
        public string description { get; set; } = "";
        public int code { get; set; } = 0;
    }
}