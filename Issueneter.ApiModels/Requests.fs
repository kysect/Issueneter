namespace Issueneter.ApiModels.Requests


type IssueFilter = string

type PullRequestFilter = string

type AddNewRepoScanRequest = {
    AccountOrOrganization: string
    Repository: string
    Issues: IssueFilter voption
    PRs: PullRequestFilter voption
}

type AddNewAccountScanRequest = {
    Account: string
    RepoCreated: bool
    IssueCreated: bool
    PullRequestCreated: bool
}