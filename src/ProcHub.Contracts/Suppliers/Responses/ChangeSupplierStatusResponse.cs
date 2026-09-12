using ProcHub.Contracts.Suppliers.Enums;

namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record ChangeSupplierStatusResponse(SupplierStatus Status);