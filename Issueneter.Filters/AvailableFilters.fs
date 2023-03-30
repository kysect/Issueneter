module Issueneter.Filters

type ScanFilter =
    | Label of title: string
    | Author of nickname: string