Знакомство с микросервисами и новыми технологиями. Компиляция большей части проектов (work in progress)
В планах для первого этапа:
- :heavy_check_mark: Создание необходимой Docker-инфраструктуры
- :heavy_check_mark: Логирование с помощью ELK (Elasticsearch, Logstash, Kibana) + Filebeat, NLog
- API-шлюз Ocelot
- Authentication service + JWT Ocelot
- Брокер сообщений RabbitMQ + MassTransit для общения мс
- REST API-микросервисы с MSSQLServer (forum-g), PostgreSQL (free-space-checker), MongoDB (check-up-money), Redis (zp) бд для разных проектов. Рефакторинг этих проектов.
- WebApps для forum-g, free-space-checker, check-up-money, zp
- HealthChecks + Watchdog для микросервисов
