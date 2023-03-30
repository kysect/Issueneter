-- +goose Up
-- +goose StatementBegin
CREATE SCHEMA IF NOT EXISTS issueneter;
CREATE TABLE IF NOT EXISTS issueneter.scans (
    id SERIAL NOT NULL
        CONSTRAINT scan_pkey PRIMARY KEY;
);
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
SELECT 'down SQL query';
-- +goose StatementEnd