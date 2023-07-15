# Issueneter

## Configuration

### Message templates

Для изменения текста сообщений нужно выставить шаблоны в конфигурационном файле:

```
{
  "TelegramOptions": {
    "IssueMessageTemplate": "изи [ишуя]({value.Url})",
    "PullRequestMessageTemplate": "[пр]({entity.Url})"
  }
}
```

Шаблоны поддерживают динамически вычисляемые поля. Для того, чтобы в шаблон вставить информацию из issue / pull request нужно указать `{value.*}`, где `*` - это название поля из issue / pull request. Список поддерживаемых динамических полей:

- Title
- Url