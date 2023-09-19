Знакомство с микросервисами и новыми технологиями. Компиляция большей части проектов (work in progress)
В планах для первого этапа:
- :heavy_check_mark: Создание необходимой Docker-инфраструктуры
- :heavy_check_mark: Логирование с помощью ELK (Elasticsearch, Logstash, Kibana) + Filebeat, NLog
- :heavy_check_mark: Authentication microservice + JWT Ocelot gw
- Брокер сообщений RabbitMQ + MassTransit для общения мс
- REST API-микросервисы с MSSQLServer (identity, forum-g), MySql (free-space-checker), MongoDB (check-up-money), PostgreSQL (zp), Redis (?) бд для разных проектов. Рефакторинг этих проектов.
- WebApps для forum-g, free-space-checker, check-up-money, zp
- HealthChecks + Watchdog для микросервисов
