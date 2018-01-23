use Pulse8TestDB;

IF EXISTS (SELECT * FROM sys.objects
			WHERE object_id = OBJECT_ID(N'dbo.Pulse8_GetMemberByID'))
			DROP PROCEDURE dbo.Pulse8_GetMemberByID
Go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Pulse8_GetMemberByID @MemberID int

AS
	BEGIN

WITH RankedDx_CTE (MemberID, DiagnosisID, Row) AS (
	SELECT MemberID
	,DiagnosisID
	,ROW_NUMBER() OVER (PARTITION BY MemberID ORDER BY DiagnosisID)
	FROM dbo.MemberDiagnosis
	),
RankedCat_CTE (DiagnosisID, DiagnosisCategoryID, SeverityRank) AS (
	SELECT DISTINCT mdx.DiagnosisID
	,DiagnosisCategoryID
	,ROW_NUMBER() OVER (PARTITION BY MemberID ORDER BY DiagnosisCategoryID)
	FROM [dbo].[MemberDiagnosis] mdx
	JOIN dbo.DiagnosisCategoryMap map on mdx.DiagnosisID = map.DiagnosisID ),
DiagnosisCategories_CTE (DiagnosisID,DiagnosisCategoryID, CategoryDuplicateRank, SeverityRank, MemberId) AS(
	SELECT DISTINCT mdx.DiagnosisID
	,DiagnosisCategoryID
	,ROW_NUMBER() OVER (PARTITION BY mdx.DiagnosisID ORDER BY DiagnosisCategoryID)
	,catSeverity.SeverityRank
	,MemberID
	FROM [dbo].[MemberDiagnosis] mdx
	JOIN RankedCat_CTE catSeverity on mdx.DiagnosisID = catSeverity.DiagnosisID )
SELECT mem.MemberID AS 'MEMBER ID'
	,mem.FirstName AS 'First Name'
	,mem.LastName AS 'Last Name'
	,dxmem.DiagnosisID AS 'Most Severe Diagnosis ID'
	,dx.DiagnosisDescription AS 'Most Severe Diagnosis Description'
	,dxcat_ranked.DiagnosisCategoryID AS 'Category ID'
	,cat.CategoryDescription AS 'Category Description'
	,cat.CategoryScore AS 'Category Score'
	,IIF(dxcat_ranked.SeverityRank IS NULL, 1, IIF(dxcat_ranked.SeverityRank = 1, 1, 0)) AS 'Is Most Severe Category'
FROM dbo.Member mem
LEFT OUTER JOIN RankedDx_CTE dxmem on mem.MemberID = dxmem.MemberID AND dxmem.Row = 1
LEFT OUTER JOIN dbo.Diagnosis dx on dx.DiagnosisID = dxmem.DiagnosisID
LEFT OUTER JOIN MemberDiagnosis dxmem_all on mem.MemberID = dxmem_all.MemberID
LEFT OUTER JOIN DiagnosisCategories_CTE dxcat_ranked on dxmem_all.DiagnosisID = dxcat_ranked.DiagnosisID AND dxcat_ranked.CategoryDuplicateRank = 1
LEFT OUTER JOIN dbo.DiagnosisCategory cat on dxcat_ranked.DiagnosisCategoryID = cat.DiagnosisCategoryID
WHERE mem.MemberID = @MemberID

END

GO