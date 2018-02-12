use Pulse8TestDB;

WITH RankedDx_CTE (MemberID, DiagnosisID, SeverityRank) AS (
	SELECT MemberID
	,DiagnosisID
	,ROW_NUMBER() OVER (PARTITION BY MemberID ORDER BY DiagnosisID)
	FROM dbo.MemberDiagnosis
	),
RankedCat_CTE (DiagnosisID, DiagnosisCategoryID, SeverityRank) AS (
	SELECT DISTINCT mdx.DiagnosisID
	,DiagnosisCategoryID
	,ROW_NUMBER() OVER (PARTITION BY MemberID ORDER BY DiagnosisCategoryID)
	FROM dbo.MemberDiagnosis mdx
	JOIN dbo.DiagnosisCategoryMap map on mdx.DiagnosisID = map.DiagnosisID ),
DiagnosisCategories_CTE (DiagnosisID,DiagnosisCategoryID, CategoryDuplicateRank, SeverityRank, MemberId) AS(
	SELECT DISTINCT mdx.DiagnosisID
	,DiagnosisCategoryID
	,ROW_NUMBER() OVER (PARTITION BY mdx.DiagnosisID ORDER BY DiagnosisCategoryID)
	,catSeverity.SeverityRank
	,MemberID
	FROM dbo.MemberDiagnosis mdx
	JOIN RankedCat_CTE catSeverity on mdx.DiagnosisID = catSeverity.DiagnosisID )
SELECT mem.MemberID AS 'Member ID'
	,mem.FirstName AS 'First Name'
	,mem.LastName AS 'Last Name'
	,dxmem.DiagnosisID AS 'Most Severe Diagnosis ID'
	,dx.DiagnosisDescription AS 'Most Severe Diagnosis Description'
	,dxcat_ranked.DiagnosisCategoryID AS 'Category ID'
	,cat.CategoryDescription AS 'Category Description'
	,cat.CategoryScore AS 'Category Score'
	,IIF(dxcat_ranked.SeverityRank IS NULL, 1, IIF(dxcat_ranked.SeverityRank = 1, 1, 0)) AS 'Is Most Severe Category'
FROM dbo.Member mem
LEFT OUTER JOIN RankedDx_CTE dxmem on mem.MemberID = dxmem.MemberID AND dxmem.SeverityRank = 1
LEFT OUTER JOIN dbo.Diagnosis dx on dx.DiagnosisID = dxmem.DiagnosisID
LEFT OUTER JOIN DiagnosisCategories_CTE dxcat_ranked on mem.MemberID = dxcat_ranked.MemberID AND dxcat_ranked.CategoryDuplicateRank = 1
LEFT OUTER JOIN dbo.DiagnosisCategory cat on dxcat_ranked.DiagnosisCategoryID = cat.DiagnosisCategoryID
