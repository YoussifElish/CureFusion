﻿namespace CureFusion.Application.Contracts.HealthArticle;

public record ArticleResponse
(
     int Id,
     string Category,
     string Title,
     string Content,
     DateTime PublishedIn
);
