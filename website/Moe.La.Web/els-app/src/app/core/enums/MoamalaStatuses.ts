export enum MoamalaStatuses {
  /// <summary>
  /// جديدة
  /// </summary>
  New = 1,

  /// <summary>
  /// محالة
  /// </summary>
  Referred = 2,

  /// <summary>
  /// مسندة
  /// </summary>
  Assigned = 3,

  /// <summary>
  /// معادة
  /// </summary>
  MoamalaReturned = 4,
}

export enum MoamalaSteps
{

    /// <summary>
    /// جديدة أو معادة إلى موزع المعاملات و المشرف
    /// </summary>
    Initial = 1,

    /// <summary>
    /// إحالة /معادة إلى إدارة عامة/منطقة
    /// </summary>
    Branch = 2,

    /// <summary>
    /// إحالة/ معادة إلى إدارة مختصة
    /// </summary>
    Department = 3,

    /// <summary>
    /// مسندة إلى موظف
    /// </summary>
    Employee = 4


}
