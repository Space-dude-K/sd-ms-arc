Знакомство с микросервисами и новыми технологиями. Компиляция большей части проектов (work in progress)
В планах для первого этапа:
- :heavy_check_mark: Создание необходимой Docker-инфраструктуры
- :heavy_check_mark: Логирование с помощью ELK (Elasticsearch, Logstash, Kibana) + Filebeat, NLog
- :heavy_check_mark: Authentication + JWT Ocelot gw
- Брокер сообщений RabbitMQ + MassTransit для коммуникации мс
- REST API-микросервисы с бд + рефакторинг:
    - MSSQLServer (identity, forum-g)
    - MySql (free-space-checker)
    - MongoDB (print-data-crawler)
    - PostgreSQL (zp),
    - Redis (?) бд для разных проектов.
- WebApps для forum-g, free-space-checker, check-up-money, zp
- HealthChecks + Watchdog для микросервисов

