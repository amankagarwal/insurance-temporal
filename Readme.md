A walkthrough on the power of Temporal, and how it simplifies writing resilient workflows. 

We will walk through a sample Insurance system with the following components:

- ClientSvc: Simulates API calls from the BFF (Backend-for-Frontend) to trigger workflows in the Policy Management Service.
- PolicyManagementSvc: Manages policies and handles workflow initiation through its APIs exposed to ClientSvc.
- Common: Stores shared data models and contracts, which can be packaged into NuGet and reused across all services.
- PremiumEngineSvc: Calculates premiums based on policy and risk data. Exposes an API to calculate the premium given all the data-points.
- BillingSvc: Acts as a worker, handling billing-related tasks and processes. Picks up tasks from the policy-billing TaskQueue which can be triggered by either signals or workflow starts, depending on the use-case.
- PaymentGatewaySvc: Integrates with payment providers to process customer payments. Loosely coupled with BillingSvc and integrates via a message broker.
- DocumentSvc: Acts as a worker, managing document generation and customer communication. Picks up tasks from the policy-docs TaskQueue which can be triggered by either signals or workflow starts, depending on the use-case.