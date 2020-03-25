-- script assumes we have a new db, first install timescale extension
CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;

-- create tables
CREATE TABLE public.event_table
(
    series_id integer NOT NULL,
    event_time timestamp without time zone NOT NULL,
    event_data real NOT NULL,
    PRIMARY KEY (series_id, event_time),
    CONSTRAINT one_event_per_series_and_timestamp UNIQUE (series_id, event_time)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.event_table
    OWNER to postgres; 	
    
-- create hypertables for timeseries tables
SELECT create_hypertable('event_table', 'event_time');

CREATE INDEX event_table_series_id_event_time_index
ON public.event_table USING btree
(series_id, event_time DESC)
TABLESPACE pg_default;