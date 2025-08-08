using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter directory path: ");
        var directory = Console.ReadLine();

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

            totalCaseStatementsInSqlCount += (caseStatementsInSql+ moreThanTwoIfElse);
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
                if (dynamicSqlExecution  > 0)
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
        Console.WriteLine($"Case statements: {totalCaseStatementsInSqlCount}: This is bad because it can point to a Open Close Principle Violation");
        Console.WriteLine($"dynamic Sql Execution: {totalDynamicSqlExecution}: This is bad because it the dacpac publish won't detect broken SQL statements");
        Console.WriteLine($"Cursors Created: {totalCursorsCreatedCount}: This is bad because Forces row-by-row processing (RBAR), which is slow and can cause locks");
        Console.WriteLine($"Update, Insert or  Merge on View: {totalWriteToViewCount}: This is bad because it the stored procs or view can be altered without interim release process detecting that the update won't work anymore");
        Console.WriteLine($"Transactions Without Error Handling: {totalTransactionsWithoutErrorHandlingCount}:This is bad because Swallows errors silently, making debugging hard and reliability low");
        Console.WriteLine($"Select Distinct: {totalSelectDistinctCount}: This is bad because it can hide duplicated results from incomplete joins");
        Console.WriteLine($"Select Top: {totalSelectTopCount}: This is bad because the selected record can be unpredictable, or unintentional altered of the orderby changes");
    }
}
