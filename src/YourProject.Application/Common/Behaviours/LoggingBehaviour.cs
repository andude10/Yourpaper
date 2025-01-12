﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using YourProject.Application.Common.Interfaces;

namespace YourProject.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? Guid.Empty;
        var userName = string.Empty;

        if (!string.IsNullOrEmpty(userId.ToString())) userName = await _identityService.GetUserNameAsync(userId);

        _logger.LogInformation("YourProject Server request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}