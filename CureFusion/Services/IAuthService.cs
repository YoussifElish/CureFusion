﻿using CureFusion.Abstactions;

namespace CureFusion.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string Email,string Password,CancellationToken cancellationToken=default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string Token,string RefreshToken,CancellationToken cancellationToken=default);
}
