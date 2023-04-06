namespace Issueneter.ApiModels.Requests


type IssueFilter = string

type PullRequestFilter = string


type AddNewAccountScanRequest = {
    Account: string
    RepoCreated: bool
    IssueCreated: bool
    PullRequestCreated: bool
}