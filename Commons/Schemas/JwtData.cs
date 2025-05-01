namespace DocumentManager.Common.Schemas
{
	public class JwtData
	{
		public Guid UserId { set; get; }
        public string Username { set; get; }
        public string Aud { set; get; }
        public string Iss { set; get; }
        public string Secret { set; get; }
    }
}

