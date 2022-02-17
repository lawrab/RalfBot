namespace SnailRacing.Ralf.Infrastrtucture
{
    public static class ResponseBaseExtensions
    {
        public static string ToResponseMessage(this ResponseBase response, string sucessMessage)
        {
            return response.HasErrors() ?
                response.ToErrorMessage() :
                $":OK: {sucessMessage}";
        }

        public static string ToErrorMessage(this ResponseBase response)
        {
            return $":no_entry: {string.Join(Environment.NewLine, response.Errors)}";
        }
    }
}
