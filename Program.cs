using System;
using System.Text.RegularExpressions;

class Program
{
    static List<(string, string)> configUsage = new List<(string, string)>();
    static void Main(string[] args)
    {

        Console.Write("Enter directory path: ");
        var directory = Console.ReadLine();

        if (args.Length == 0)
        {
            ScanForSmells(directory);
            ScanForConfigurations(directory);
        }
        else
        {
            var template = args[0];
            bool exactMatch = false;
            if (args.Length > 1)
            {
                exactMatch = true;
            }
            ScanForDuplicates(directory, template, exactMatch);
        }
    }

    private static void ScanForConfigurations(string? directory)
    {        
        string[] configurationFlags = {
            "AllowEngagementDateChange",
            "ShowEmployeeClocksTSE",
            "AdHocPaymentLimit",
            "AdHocPaymentLimit_AL",
            "AdHocPaymentLimit_FA",
            "AdHocPaymentLimit_FA01",
            "AdHocPaymentLimit_FA02",
            "AdHocPaymentLimit_FA03",
            "AdHocPaymentLimit_FA04",
            "AdHocPaymentLimit_FA05",
            "AdHocPaymentLimit_FA06",
            "AdHocPaymentLimit_FA07",
            "AdHocPaymentLimit_FA08",
            "AdHocPaymentLimit_FA09",
            "AdHocPaymentLimit_FL",
            "AdHocPaymentLimit_HA",
            "AdHocPaymentLimit_HA01",
            "AdHocPaymentLimit_LV",
            "AdHocPaymentLimit_MC1",
            "AdHocPaymentLimit_NRM",
            "AdHocPaymentLimit_OT",
            "AdHocPaymentLimit_OT_A",
            "AdHocPaymentLimit_OT_B",
            "AdHocPaymentLimit_OT_C",
            "AdHocPaymentLimit_OT_D",
            "AdHocPaymentLimit_OT_E",
            "AdHocPaymentLimit_OT_H",
            "AdHocPaymentLimit_OT_K",
            "AdHocPaymentLimit_OT_L",
            "AdHocPaymentLimit_OT_M",
            "AdHocPaymentLimit_OT_NRM",
            "AdHocPaymentLimit_OT_P",
            "AdHocPaymentLimit_OT_Q",
            "AdHocPaymentLimit_OT_R",
            "AdHocPaymentLimit_OT_V",
            "AdHocPaymentLimit_OT_W",
            "AdHocPaymentLimit_OT_X",
            "AdHocPaymentLimit_OT_Y",
            "AdHocPaymentLimit_OT_Z",
            "AdHocPaymentLimit_OT1.5",
            "AdHocPaymentLimit_OT2.0",
            "AdHocPaymentLimit_PP01",
            "AdHocPaymentLimit_PP02",
            "AdHocPaymentLimit_PP03",
            "AdHocPaymentLimit_PP04",
            "AdHocPaymentLimit_QOH2",
            "AdHocPaymentLimit_QOH3",
            "AdHocPaymentLimit_QOH4",
            "AdHocPaymentLimit_SAT",
            "AdHocPaymentLimit_SL",
            "AdHocPaymentLimit_SUN",
            "AdHocPaymentNumberLimit",
            "AdvancedRosterShowRequisitions",
            "AdvancedRosterShowRequisitionStatus",
            "AllowAdjustContractHours",
            "AllowAdvRequisitionComplete",
            "AllowAssignmentDateExtension",
            "AllowAssignmentEdit",
            "AllowAutoClocksForFuture",
            "AllowAutoClocksFuture",
            "AllowBuzzClockMultiAss",
            "AllowConfOTRule_New",
            "AllowContractHoursProRata",
            "AllowEmployeeExtraInfo",
            "AllowEmployeeExtraInfo",
            "AllowEngagementDateChange",
            "AllowGradeLookupOnAssignment",
            "AllowInvalidShiftsCC",
            "AllowLeaveImportPDLReRoster",
            "AllowLockedPayrollChange",
            "AllowLockedPayrollChangeFlexi",
            "AllowOTPeriodSecondThresholdAdjust",
            "AllowOwnLeaveEdit",
            "AllowOwnTimesheetEdit",
            "AllowPaymentRateEdit",
            "AllowPayrollTransferLeaveTransactions",
            "AllowPayrollTransferWhenMatchedByEmployee",
            "AllowPeriodTimesheetView",
            "AllowPONumberChanges",
            "AllowProportionalContractHours",
            "AllowRequisitionClientTeamUpdate",
            "AllowRequisitionExtension",
            "AllowRequisitionTeamChange",
            "AllowRosterMaxWeeksEdit",
            "AllowRosterMaxWeeksEdit",
            "AllowUnfilledRequisitionClientTeamUpdate",
            "ApplicationVersion",
            "ApprovalCheckBudget",
            "ApprovalFilterPayroll",
            "ApprovalFilterPayroll",
            "ApprovalFilterVendor",
            "ApprovalRateGroup",
            "AssetAllowEditIssueDate",
            "AssetMaxIssuedateBackdate",
            "AssignmentDashboardShowCreate",
            "AssignmentsForClockers",
            "AssignResource",
            "AuthorisationRateGroup",
            "AuthorizationRateGroup",
            "AutoConfOTRule_New",
            "AutoConfShifts_New",
            "AutoConfShifts_New",
            "AutoConfShifts_Update",
            "AutoConfShifts_Update",
            "AutoGenerateProjectCode",
            "BiometricAccessCutoff",
            "blnAllowSelectionForESS",
            "blnShowBalanceInESS",
            "BlockApprovalNoShows",
            "BlockAssignmentExtension",
            "BlockAssignmentInactiveClocks",
            "BlockAuthIfAssignmentReasonMissing",
            "BlockBaseRateDirectEdit",
            "BlockEmployeeInactive",
            "BlockLeaveDelete",
            "BlockLeaveDelete",
            "BlockModifyDatesLeaveNotCancelled",
            "BlockNewTSEClocks",
            "BlockNonTemplateShifts",
            "BlockOverlappingClocks",
            "BlockOwnLeaveManagement",
            "BlockOwnLeaveManagement",
            "BlockOwnTSEClocks",
            "BlockPHRosterDelete",
            "BreathalyserTestingGroup",
            "BudgetEntryShiftMandatory",
            "BuzzPEClocks",
            "BuzzSessionTimeoutDays",
            "Calendar_0",
            "Calendar_1",
            "Calendar_2",
            "CalendarEditEnable",
            "CapMaxAllowances",
            "CheckEmployeeRates",
            "CheckMasterCostCentre",
            "Classic_DB",
            "Classic_LoginCheck",
            "Classic_URL",
            "ClocksAcrossEmployees",
            "CompanyPasswordHistoryCheck",
            "CompanyPasswordMaxLengthReq",
            "CompanyPasswordMinLengthReq",
            "CompanyPasswordMinLowerCaseCharReq",
            "CompanyPasswordMinNumericCharReq",
            "CompanyPasswordMinSpecialCharReq",
            "CompanyPasswordMinUpperCaseCharReq",
            "CompanyPasswordPersonalInfoCheck",
            "Cost Centre",
            "DBVersion",
            "DefaultCalendar",
            "defaultleaveenabled",
            "DefaultPayroll",
            "DevelopmentMode",
            "DisableAdHocRole",
            "DisableAssignmentsAdhocProcess",
            "DisableEmployeeImport",
            "DisableRequisitionBudgetRosterCheck",
            "DisableShiftPlanOvertimeRule",
            "DisableStandardShiftRosterShiftPlan",
            "DisciplinaryIssuedByEditable",
            "DivisionAutoConf",
            "DivisionAutoConf",
            "DriversLicenceInfoCategory",
            "EmpImport_MoveAssignments",
            "EmployeeForceAllowAssignmentsOn",
            "EmployeeOnboardingWorkFlowCode",
            "EmployeeSearchColumns",
            "EmployeeStatus",
            "EmploymentEquityOccupationalLevelCode",
            "EmpWizValidateOnlyMatchOnId",
            "EnableSLLeave",
            "ETIRequistionSearchCol",
            "ETIRequistionSearchDetailCol",
            "Exclude_PayrollCodes",
            "ExecuteWorkflowImmediately",
            "ExpiredAlertCategory",
            "ExportEndpointName",
            "ExportPayrollCombined",
            "ExportPayrollCombinedByFileNo",
            "ExportPayrollCombinedScheduledReportingPayroll",
            "ExportPayrollExtractedAssOnly",
            "ExportPayrollSage300UseCustom2ForCC",
            "ExportSingleDayLeaveTransactions",
            "ExternalHRMaster",
            "ExternalLinkLabel",
            "ExternalLinkURL",
            "ExtractSeparateFTP",
            "ExtractSeqNoSeparate",
            "ExtractSequenceNumber",
            "ForceShiftPlan",
            "GenerateExtractSequence",
            "GenerateLeaveReference",
            "GenerateRatesetOnFill",
            "GenericLargeReportFontSize",
            "HideAssigmentReason",
            "HideAssignmentReason",
            "HideIdNumbers",
            "HideResourcePayroll",
            "HideShiftplanIndicator",
            "IgnoreDirection",
            "ImportCardNumber",
            "ImportFolder",
            "intLeaveCommentOption",
            "JobCatMandatoryTempPayroll",
            "LandingDBName",
            "LandingImportKey",
            "LandingServer",
            "Leave_essenabled",
            "LeaveBalanceUpdatedDaily",
            "LeaveCompanyExportCode",
            "LeaveSequenceNumber",
            "LegacyLeaveBalanceCheck",
            "MailProfile",
            "MaintenanceMode",
            "MaxHoursPerPeriod",
            "MaxRequisitionBatch",
            "MaxRequisitionDays",
            "MobileApp1",
            "MonitorPE",
            "Next_MaintenanceRun",
            "NonbioShowCostCenter",
            "NonbioShowCostCenterCode",
            "NonbioShowOvertimeThreshold",
            "NoShortTimeOutClock",
            "NotfyReport_REQ_CANCEL_CLIENT",
            "NotfyReport_REQ_FILL",
            "NotfyReport_REQ_NEW_CLIENT",
            "NotifyReport_Extract",
            "NotifyReport_Leave",
            "NotifyReport_REQ_ACCEPT_OVERRIDE",
            "NotifyReport_REQ_BUDGET_OVERRIDE",
            "NotifyReport_REQ_CANCEL_CLIENT",
            "NotifyReport_REQ_FIL",
            "NotifyReport_REQ_FILL",
            "NotifyReport_REQ_FILL_CLIENT",
            "NotifyReport_REQ_NEW_CLIENT",
            "NotifyReport_REQ_NEW_CLIENT",
            "NotifyReport_REQ_REQUEST_OVERRIDE",
            "NotifyReport_REQ_USSD_ACCEPT",
            "Orgstructure",
            "OverrideInterimFlag",
            "OverridePlannedWeeks",
            "OverrideShiftHoursWithStdLeaveHours",
            "Payroll_ShowEmpNumber",
            "Payroll_ShowEmpNumber",
            "PayrollApprovalBlockOrWarning",
            "PayrollExportUpdateRateCodes",
            "payrollprioritycolumnname",
            "PayrollShowEmpNumber",
            "PayrollXferActiveReset",
            "PE_CalcUntil",
            "PE_Days_Future",
            "PE_Enabled",
            "PE_Weeks_Config",
            "PE_Weeks_Next",
            "PeriodLimitAbsenceRates",
            "PeriodLimitNormalTimeRates",
            "PeriodTimesheetFilterWeekly",
            "PeriodTimesheetProject",
            "PeriodTimesheetSelectProject",
            "PeriodTimesheetSelectRateset",
            "PeriodTimesheetSelectReference",
            "PH1.0RateType",
            "PreferVendorManagement",
            "PreferVendorManagment",
            "Project_ActivityRequired",
            "Project_DeptRequired",
            "Project_ProjRequired",
            "ProjectLoad_NumberOfMonths",
            "PSIRAGradeInfoCategory",
            "RegNo",
            "Report_BreakDeduct",
            "Report_CostCentre_Dispay",
            "Report_CostCentre_Display",
            "Report_CustomFilter",
            "Report_PrimarySkillCode",
            "Report_RateGroup105",
            "Report_RegionDivGroup",
            "Report_SecondarySkillCode",
            "Report_SES_Reference",
            "Report_ShowJobCategory",
            "Report_ShowJobCategory",
            "Report_ShowJobGrade",
            "Report_SiteAtLevel",
            "Report_Team_Dispay",
            "Report_Team_Display",
            "Report_Team_Display",
            "Report_TertiarySkillCode",
            "ReportFolder",
            "Reports: Maximum date range",
            "ReportServer",
            "RequireJobCategory",
            "RequireJobGrade",
            "RequireReasonForShiftChange",
            "RequireRequisitionJobCat",
            "RequisitionBudgetReportCode",
            "RequisitionRatesetFromGrade",
            "RequisitionRequiresRateset",
            "RequisitionUseDivisionDefaultCalenda",
            "RequisitionUseDivisionDefaultCalendar",
            "RequisitionUssdExpiryMinutes",
            "RestoreTimestamp",
            "RM_Calendar",
            "RM_CheckAuthorisationPO",
            "RM_Database",
            "RM_RequirePO",
            "RM_URL",
            "RosterAdvancedShowAssignmentDescription",
            "RosterDefault_PH",
            "RosterDetail",
            "RosterEnforceRequirementCheck",
            "RosterOvertimeRategroup",
            "RoundQtyDigits",
            "RsoterDetail",
            "SABClockImport",
            "SABMaterialValidation",
            "SABPOImport",
            "SAGE300CompanyCode",
            "SAGE300CompanyRuleCode",
            "Sage300Department",
            "Sage300DisabilityStatus",
            "Sage300GLGroup",
            "SAGE300GLGroupKey",
            "Sage300JobGrade",
            "Sage300JobTitle",
            "Sage300LeaveExportBatchSize",
            "Sage300LeaveExportMaxRetries",
            "Sage300LeaveExportRetryBackoffLimit",
            "Sage300LeaveExportRetryBackoffMinutes",
            "Sage300LeavePolicyCode",
            "Sage300NatureOfContract",
            "Sage300PayPoint",
            "SAGE300PayPointKey",
            "Sage300Project",
            "Sage300RemEarningDef",
            "Sage300RemunerationDefHeader",
            "Sage300RunDefCode",
            "Sage300SendCountry",
            "Sage300TaxCalculation",
            "Sage300TaxStatus",
            "Sage300UIFStatusCode",
            "ScheduleImportPriorityAbsence",
            "ScheduleImportPriorityShift",
            "SD6SpecialAllowances",
            "Segment_Dir_Priority",
            "SendSMS",
            "SetBaseOrgUnit",
            "setfilesize",
            "ShowAdditioinalPayments",
            "ShowAdditionalPayments",
            "ShowAdditionaPayment",
            "ShowAdvancedLeave",
            "ShowAdvancedRequisitionWorkflow",
            "ShowApproveConfirm",
            "ShowAssignResource",
            "ShowAuthoriseConfirm",
            "ShowBioClocks_106",
            "ShowCopyPaste",
            "ShowDisciplinaryPrintBtn",
            "ShowEmployeeClocksTSE",
            "ShowEmployeeWizard",
            "ShowEmployeeWizard",
            "ShowETI",
            "ShowExractBatchId",
            "ShowExtractBatchId",
            "ShowExtractConfirm",
            "ShowJobCategory",
            "ShowJobCategoryGradeItemOnRates",
            "ShowJobItemOnConfig",
            "ShowManualRosterFlag",
            "ShowNonBioAutoClocks",
            "ShowOrgCode_Prefix",
            "ShowOrgCode_Suffix",
            "ShowOvertime",
            "ShowPayrollPriority",
            "ShowPeriodOvertime",
            "ShowPeriodTimesheet",
            "ShowProjectsAsJobs",
            "ShowRequirementDetail",
            "ShowRequirementRoster",
            "ShowRequisition",
            "ShowRequisitionAddress",
            "ShowRequisitionCalendar",
            "ShowRequisitionGrade",
            "ShowRequisitionJobCat",
            "ShowRequisitionJobCategory",
            "ShowRequisitionModify",
            "ShowRequisitionPayroll",
            "ShowRequisitionRateset",
            "ShowRequisitionWizard",
            "ShowRequistionModify",
            "ShowRosterOvertimeCheck",
            "ShowRosterTotalHours",
            "ShowTeamRoster",
            "ShowTotalHours",
            "ShowTSE",
            "ShowTSEOvertimeFlags",
            "ShowTSEStickyClocks",
            "ShowTSEStickyClocks",
            "ShowVendorEmployeeId",
            "SkipClonedAssignmentStartChange",
            "SkipLeaveImportOnPH",
            "SMSAccount",
            "SMSAccountPassword",
            "SMSFTP",
            "SMSPassword",
            "SMSPort",
            "SMSSerial",
            "SMSServer",
            "SMSServerTable",
            "SorterProductionRateType",
            "StatusAdvancedTimesheet",
            "StatusDeductBreaks",
            "statusshowadvancedleave",
            "StatusShowAssignmentNumber",
            "StatusShowContractHours",
            "StatusShowEmployeeAssigmentNumber",
            "StatusShowEmployeeAssignmentNumber",
            "StatusShowEmployeeCalendar",
            "StatusShowEmployeeIdNumber",
            "StatusShowEmployeeIdStrin",
            "StatusShowEmployeeIdString",
            "StatusShowEmployeeJobCategory",
            "StatusShowEmployeePayroll",
            "StatusShowJobCategory",
            "StatusShowJobGrade",
            "StatusShowLeaveAlias",
            "StatusShowOvertimeThreshold",
            "StatusShowPayroll",
            "StatusShowRosteredDays",
            "StatusShowRosteredHours",
            "StatusShowRosteredLeave",
            "StatusShowRoteredHours",
            "StatusShowVendor",
            "SundayOTO.5RateType",
            "SupportEmail",
            "SupportIconIndex",
            "SupportUrl",
            "SuppressSenderDetailUssd",
            "SysPasswordExpDays",
            "SysPasswordGenSpecialChar",
            "SysPasswordMaxAttemptsLock",
            "SysPasswordMaxLengthReq",
            "SysPasswordMinLengthReq",
            "SysPasswordMinLowerCaseCharReq",
            "SysPasswordMinNumericCharReq",
            "SysPasswordMinSpecialCharReq",
            "SysPasswordMinUpperCaseCharReq",
            "SystemName",
            "TaskConfiguration",
            "TeamCustom1",
            "TelematicsImport",
            "TemplateDivision",
            "TerminateVendorAssignment",
            "TimesheetChangeListItemRequired",
            "TrainingConfiguration",
            "TSEAllowModifyOwnClock",
            "TseboAppendSuffixToTransCode",
            "TseboTempPayrollCode",
            "TSERe1asonMandatory",
            "TSEReasonMandatory",
            "UnmatchedPayrollForImport",
            "UseRecruitmentAssignmentReason",
            "UseTrackingPayrollEmployeeWizard",
            "UssdNumberWithInstance",
            "VatRegNo",
            "Virt_Emp",
            "WeekdayStart",
            "WinSMSFTP",
            "ZonePercentage"
        };        
        
        configUsage.Clear();               

        var csFiles = Directory.GetFiles(directory, "*.sql", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            Console.WriteLine($"Scanning {file}...");            

            var sql = File.ReadAllText(file);

            foreach (string flagsPattern in configurationFlags)
            {
                
                string pattern = $"\\bIF\\b[\\s\\S]*?\\b({flagsPattern})\\b";

                var matches = Regex.Matches(sql, pattern, RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    AddMatch(file, flagsPattern);
                }
            }
        }
        var mostConfiguredFiles = configUsage.GroupBy(x => x.Item1).OrderByDescending(g => g.Count());
        mostConfiguredFiles.Take(10).ToList().ForEach(x => Console.WriteLine($"{x.Key} has {x.Count()} configuration paths"));

        var mostUsedConfiguration = configUsage.GroupBy(x => x.Item2).OrderByDescending(g => g.Count());
        mostUsedConfiguration.Take(10).ToList().ForEach(x => Console.WriteLine($"{x.Key} is used in {x.Count()} Places"));

        var unusedConfigs = configurationFlags.Except(configUsage.GroupBy(x => x.Item2).Select(g => g.Key).ToList());
        unusedConfigs.ToList().ForEach(x => Console.WriteLine($"Unused Config : {x}"));
    }

    private static void AddMatch(string file, string flagsPattern)
    {
        configUsage.Add(new(file, flagsPattern));
    }

    private static string[] exemptLines = { "USE [V3-05]", "USE [V3-05]", "GO", "SET ANSI_NULLS ON", "SET QUOTED_IDENTIFIER ON", "@intUserId INT,", "@intCompanyId INT,",
        "@intSessionId INT,", "-- History", "AS", "BEGIN", "END", "SET NOCOUNT ON", "@strOutputMessage VARCHAR(MAX) OUTPUT,", "DECLARE @intRet INT,", "DECLARE @intRet INT",
        "RETURN @intRet", "RETURN -99" ,"SET @strOutputMessage = 'Invalid api parameter'", "-- Return results", "@strOutputMessage = @strOutputMessage OUTPUT",
        "-- Call base proc to perform actual checking", "",string.Empty };
    private static void ScanForDuplicates(string? directory, string template, bool exactMatch)
    {
        Console.WriteLine("\nScanning .cs files...\n");

        var threshold = exactMatch ? 97D : 50D;
        var csFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        foreach (var fileouter in csFiles)
        {
            var contentouter = File.ReadAllText(fileouter);
            var linesouter = contentouter.Split("\r\n");
            foreach (var file in csFiles)
            {
                if (fileouter != file)
                {
                    var content = File.ReadAllText(file);
                    var lines = content.Split("\r\n");
                    if (lines.Length == 1)
                    {
                        lines = content.Split("\r");
                    }
                    foreach (var line in lines)
                    {
                        foreach (var lineOuter in linesouter)
                        {
                            if (exemptLines.Contains(lineOuter.TrimStart()) || exemptLines.Contains(line.TrimStart()))
                            { continue; }
                            var similarity = CalculateSimilarity(line.TrimStart().TrimEnd(), lineOuter.TrimStart().TrimEnd());

                            if (similarity > threshold)
                            {
                                Console.WriteLine(line);
                                Console.WriteLine($"-   {similarity:F2}   -------{fileouter}----------{file}-----------------   {similarity:F2}   -");
                                Console.WriteLine(lineOuter);
                            }
                        }
                    }
                }
            }
        }
    }

    private static void ScanForSmells(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
        {
            Console.WriteLine("Invalid directory.");
            return;
        }

        int totalAsyncVoidCount = 0;
        int totalEmptyCatchCount = 0;
        int totalCaughtRethrowCount = 0;
        int totalSwitchStatementsCount = 0;
        int totalCaseStatementsInSqlCount = 0;
        int totalDynamicSqlExecution = 0;
        int totalCursorsCreatedCount = 0;
        int totalWriteToViewCount = 0;
        int totalTransactionsWithoutErrorHandlingCount = 0;
        int totalSelectDistinctCount = 0;
        int totalSelectTopCount = 0;

        Console.WriteLine("\nScanning .cs files...\n");

        var csFiles = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            var content = File.ReadAllText(file);

            int asyncVoidCount = Regex.Matches(content, @"async\s+void\s+\w+\s*\(", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int emptyCatchCount = Regex.Matches(content, @"catch\s*(\([^\)]*\))?\s*\{\s*(\/\/[^\n]*\s*)*\}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int caughtRethrowCount = Regex.Matches(content, @"catch\s*\(\s*(\w+)\s+\w+\s*\)[^}]*throw\s+\w+\s*;", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int switchStatementCount = Regex.Matches(content, @"switch\s*\([^\)]*\)\s*\{[^}]*\}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int moreThanTwoIfElse = Regex.Matches(content, @"(?i)(?:else\s+if\s*\([^\)]*\)\s*\{[^}]*\}){2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline).Count;

            totalAsyncVoidCount += asyncVoidCount;
            totalEmptyCatchCount += emptyCatchCount;
            totalCaughtRethrowCount += caughtRethrowCount;
            totalSwitchStatementsCount += (switchStatementCount + moreThanTwoIfElse);

            if (asyncVoidCount > 0 || emptyCatchCount > 0 || caughtRethrowCount > 0 || (switchStatementCount + moreThanTwoIfElse) > 0)
            {
                Console.WriteLine($"{Path.GetFileName(file)}:");
                if (asyncVoidCount > 0)
                    Console.WriteLine($"  - async void methods: {asyncVoidCount}");
                if (emptyCatchCount > 0)
                    Console.WriteLine($"  - empty catch blocks: {emptyCatchCount}");
                if (caughtRethrowCount > 0)
                    Console.WriteLine($"  - caught exception rethrows: {caughtRethrowCount}");
                if (switchStatementCount > 0)
                    Console.WriteLine($"  - switch or longe if else statements: {(switchStatementCount + moreThanTwoIfElse)}");
            }
        }

        csFiles = Directory.GetFiles(directory, "*.sql", SearchOption.AllDirectories);
        foreach (var file in csFiles)
        {
            var content = File.ReadAllText(file);

            int caseStatementsInSql = Regex.Matches(content, @"(?i)\bCASE\b.*?\bEND\b", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int dynamicSqlExecution = Regex.Matches(content, @"(?i)\bEXEC(?:UTE)?\b\s*(?:sp_executesql|\(@?.*?\)|'[^']*')", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int cursorCreated = Regex.Matches(content, @"(?i)\bDECLARE\b\s+\w+\s+\bCURSOR\b\s+\bFOR\b", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int WriteToView = Regex.Matches(content, @"(?i)\b(?:merge\s+into|update|insert\s+into)\s+(?:\[\w+\]\.)?(?:\[\s*vw\w+\s*\]|vw\w+)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int transactionsWithoutErrorHandling = Regex.Matches(content, @"(?i)\bbegin\s+tran\b|\bcommit\b(?!.*rollback)|\brollback\b(?!.*commit)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int selectDistinct = Regex.Matches(content, @"(?i)\bselect\s+distinct\b", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int selectTop = Regex.Matches(content, @"(?i)\bselect\s+top\b", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;
            int moreThanTwoIfElse = Regex.Matches(content, @"(?i)(?:(?:ELSE\s+IF|ELSEIF)\s+[^\s]+\s+THEN[^;]*;?){2,}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled).Count;

            totalCaseStatementsInSqlCount += (caseStatementsInSql + moreThanTwoIfElse);
            totalDynamicSqlExecution += dynamicSqlExecution;
            totalCursorsCreatedCount += cursorCreated;
            totalWriteToViewCount += WriteToView;
            totalTransactionsWithoutErrorHandlingCount += transactionsWithoutErrorHandling;
            totalSelectDistinctCount += selectDistinct;
            totalSelectTopCount += selectTop;

            if ((caseStatementsInSql + moreThanTwoIfElse) > 0 || dynamicSqlExecution > 0 || cursorCreated > 0 || WriteToView > 0 || transactionsWithoutErrorHandling > 0 || selectDistinct > 0 || selectTop > 0)
            {
                Console.WriteLine($"{Path.GetFileName(file)}:");

                if (caseStatementsInSql > 0)
                    Console.WriteLine($"  - switch statements: {(caseStatementsInSql + moreThanTwoIfElse)}");
                if (dynamicSqlExecution > 0)
                    Console.WriteLine($"  - dynamic Sql Execution: {dynamicSqlExecution}");
                if (cursorCreated > 0)
                    Console.WriteLine($"  - Cursors Created: {cursorCreated}");
                if (WriteToView > 0)
                    Console.WriteLine($"  - Update, Insert or  Megre on View: {WriteToView}");
                if (transactionsWithoutErrorHandling > 0)
                    Console.WriteLine($"  - Transactions Without Error Handling: {transactionsWithoutErrorHandling}");
                if (selectDistinct > 0)
                    Console.WriteLine($"  - Select Distinct: {selectDistinct}");
                if (selectTop > 0)
                    Console.WriteLine($"  - Select Top: {selectTop}");
            }
        }

        Console.WriteLine("\n--- Summary ---");
        Console.WriteLine($"Total async void methods: {totalAsyncVoidCount} : This is bad because exceptions thrown in these methods cannot be caught in calling methods");
        Console.WriteLine($"Total empty catch blocks: {totalEmptyCatchCount}: This is bad because Swallows errors silently, making debugging hard and reliability low");
        Console.WriteLine($"Caught exception rethrows: {totalCaughtRethrowCount}: This is bad because it resets the stack trace, hiding the original source of the error");
        Console.WriteLine($"switch statements: {totalSwitchStatementsCount}: This is bad because it can point to a Open Close Principle Violation");
        Console.WriteLine($"Case statements: {totalCaseStatementsInSqlCount}: Sees Paragraph below");
        Console.WriteLine($"dynamic Sql Execution: {totalDynamicSqlExecution}: This is bad because it the dacpac publish won't detect broken SQL statements");
        Console.WriteLine($"Cursors Created: {totalCursorsCreatedCount}: This is bad because Forces row-by-row processing (RBAR), which is slow and can cause locks");
        Console.WriteLine($"Update, Insert or  Merge on View: {totalWriteToViewCount}: This is bad because it the stored procs or view can be altered without interim release process detecting that the update won't work anymore");
        Console.WriteLine($"Transactions Without Error Handling: {totalTransactionsWithoutErrorHandlingCount}:This is bad because Swallows errors silently, making debugging hard and reliability low");
        Console.WriteLine($"Select Distinct: {totalSelectDistinctCount}: This is bad because it can hide duplicated results from incomplete joins");
        Console.WriteLine($"Select Top: {totalSelectTopCount}: This is bad because the selected record can be unpredictable, or unintentional altered of the orderby changes");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("A heavy reliance on large CASE statements in stored procedures often means the raw data itself is incomplete, \r\nambiguous, or context-dependent, and the “real meaning” of a value is only determined at query time by\r\napplying business rules.\r\nThat has a few big implications:\r\n* Business meaning is time-sensitive\r\nIf the stored procedure logic changes in the future,\r\nthe same historical data might produce different results,\r\nmaking it hard to reproduce past reports accurately.\r\n* Loss of data integrity\r\nIf important classification or derivation rules are not stored alongside the data,\r\nyou can’t guarantee that you’re interpreting old data the same way it was when first entered.\r\n* Data portability issues\r\nIf rules live in stored procedures instead of the data model, moving the data to another\r\nsystem requires reimplementing all that hidden logic.\r\n* Harder auditing\r\nHistorical business decisions may be impossible to verify without knowing exactly\r\nwhich version of the rules was applied at the time.\r\nIn other words, the CASE statements are compensating for missing or implicit data,\r\nand that can be risky if the rules evolve");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static double CalculateSimilarity(string source, string target)
    {
        if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target))
            return 100.0;
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
            return 0.0;

        int distance = LevenshteinDistance(source, target);
        int maxLen = Math.Max(source.Length, target.Length);
        return (1.0 - (double)distance / maxLen) * 100.0; // returns a % similarity
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (int i = 0; i <= n; d[i, 0] = i++) { }
        for (int j = 0; j <= m; d[0, j] = j++) { }

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[n, m];
    }

}
