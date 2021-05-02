csbx
    - connections 
        - list
        - add
            -   ServiceBusNamespaceType connectionStringType,
            -   string connectionString,
            -   string uri,
            -   string ns,
            -   string servicePath,
            -   string name,
            -   string key,
            -   string stsEndpoint,
            -   TransportType transportType,
            -   bool isSas = false,
            -   string entityPath = "",
            -   bool isUserCreated = false
        - remove
        - purge
    +++
    - connectionString
    - connection, c
    +++
    - info
    - topics
        - list
        - info
        - send
            - name, n
            - message, m
            - messagePath, M
            - format, F
            - header, H key=value - 0..*
            - fromFile, F
        - subscriptions
            - list
            - name, n
                - info
                - peek
                    - list
                        - limit (10)
                    - index
                - create
                - remove
                - deadletter
                    - peek
                        - list
                            - limit (10)
                        - index

csbx topics send -c mySb -n MarketDayEndEvent -m '{ "payload": "gogogogo" }' -F string -H 'ContentType=application/json' 