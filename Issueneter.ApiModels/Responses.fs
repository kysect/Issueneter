namespace Issueneter.ApiModels.Responses

open System

type ScanStatus =
    | Scheduled
    | Disabled
    | Failed

type ScanDetails = {
    Created: DateTime
    SuccessfulRuns: int
    Status: ScanStatus
}

type RepositoryScan = {
    AccountOrOrganization: string
    Repository: string
    Details: ScanDetails
}

type AccountScan = {
    Account: string
    Details: ScanDetails
}

type ScansResponse = {
    AccountScans: AccountScan array
    RepositoryScans: RepositoryScan array
}

type ScanType =
    | Repository
    | Account

type ScanResponse = {
    ScanId: int
    ScanType: ScanType
    AccountScan: AccountScan voption
    RepositoryScan: RepositoryScan voption
}