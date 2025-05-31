namespace CureFusion.Infrastructure.Helpers
{
    public static class EmailBodyBuider
    {

        private const string _templateBasePath = @"C:\\Users\\Elish\\source\\repos\\CureFusion\\CureFusion.Infrastructure\\Templates";
        public static string GenerateEmailBody(string tempelate, Dictionary<string, string> tempelateModel)
        {

            var tempelatePath = Path.Combine(_templateBasePath, $"{tempelate}.html");
            var streamReader = new StreamReader(tempelatePath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();
            foreach (var item in tempelateModel)
            {
                body = body.Replace(item.Key, item.Value);
            }

            return body;

        }
    }
}
