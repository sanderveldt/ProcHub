using ProcHub.Contracts.Suppliers.Enums;

namespace ProcHub.Contracts.Suppliers.Requests;

public sealed record ChangeSupplierStatusRequest(SupplierStatus Status);