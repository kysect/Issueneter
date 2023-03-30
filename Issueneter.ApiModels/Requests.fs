namespace Issueneter.ApiModels.Requests


type LabelFilter = {
    Title: string
}

type AuthorFilter = {
    Nickname: string
}

type IssueFilter = {
    LabelFilters: LabelFilter array
    AuthorFilters: AuthorFilter array
}

type PullRequestFilter = {
    LabelFilters: LabelFilter array
    AuthorFilters: AuthorFilter array
}

type AddNewRepoScanRequest = {
    AccountOrOrganization: string
    Repository: string
    Issues: IssueFilter array
    PRs: PullRequestFilter array
}

type AddNewAccountScanRequest = {
    Account: string
    RepoCreated: bool
    IssueCreated: bool
    PullRequestCreated: bool
}