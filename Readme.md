# Insurance Workflow with Temporal
This repository demonstrates a simplified insurance application built with multiple service, using Temporal to manage complex workflows. It includes the following services:

- **ClientSvc:** Simulates API calls from the BFF (Backend-for-Frontend) to trigger workflows in the Policy Management Service.
- **PolicyManagementSvc:** Manages policies and handles workflow initiation through its APIs exposed to ClientSvc.
- **Common:** Contains shared data models and contracts, which can eventually be packaged into NuGet and reused across all services.
- **PremiumEngineSvc:** Calculates premiums based on policy and risk data. Exposes an API to calculate the premium given all the data-points.
- **BillingSvc:** Manages billing-related tasks and processes. Picks up tasks from the policy-billing TaskQueue which can be triggered by either signals or workflow starts, depending on the use-case.
- **PaymentGatewaySvc:** Integrates with payment providers to process customer payments. Currently integrates with BillingSvc via an API, and returns a Success boolean in the response.
- **DocumentSvc:** Manages document generation and customer communication. Picks up tasks from the policy-docs TaskQueue which can be triggered by either signals or workflow starts, depending on the use-case.

## Walkthrough
For a detailed walkthrough, check out the accompanying Medium article:

[Streamlining Auto Insurance Workflows with Temporal and .NET](https://medium.com/@aman-k-agarwal/streamlining-auto-insurance-workflows-with-temporal-and-net-b185700940b5)
