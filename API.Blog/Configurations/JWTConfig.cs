namespace Blog.API.Configurations;

public class JWTConfig
{
    public readonly string? _secretKey;

    public JWTConfig(string secretKey)
    {
        _secretKey = secretKey;
    }
}
