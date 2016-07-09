

DECLARE @IdServicio INT;
 DECLARE @IdRelacion INT
 DECLARE @Referencia VARCHAR(20)
 SELECT @Referencia = '0000000007'


/*
SELECT REF.IdReferencia, REF.Referencia, SERV.IdsServId, SERV.ChrServDesc, REF.IdRelacion, ValorServicio, SUM(ValorServicio) OVER (PARTITION BY SERVREF.IdReferencia) ValorServiciosTotales
FROM TblReferenciasRecibos REF
INNER JOIN Servicios_ReferenciasRecibos SERVREF ON REF.IdReferencia = SERVREF.IdReferencia
INNER JOIN TblServicio SERV ON SERV.IdsServId = SERVREF.IdServicio
WHERE REF.Referencia = @Referencia
*/
		SELECT @IdServicio = IdServicio, 
				@IdRelacion = IdRelacion
		FROM BDAdmin.dbo.TblReferenciasRecibos REF
		INNER JOIN BDAdmin.dbo.Servicios_ReferenciasRecibos SERVREF ON REF.IdReferencia = SERVREF.IdReferencia 
		WHERE REF.Referencia = @Referencia


	IF @IdServicio = 138 --MATRÍCULA
		BEGIN
			SELECT IdsMatricId, IdsEstProgId, ChrTerNomCompleto, ChrCodCarnet, AliasPrograma, IntPerAcadId, IntMatricEdo
			FROM ViceAcad.DBO.TblMatric 
			INNER JOIN ViceAcad.DBO.TrelEstProg ON IdsEstProgId = IntEstProgId
			INNER JOIN Contabilidad.DBO.TblTercero ON TblTercero.IdsTerceroId = TrelEstProg.IntTerEstudId  
			INNER JOIN ViceAcad.DBO.TblPrograma ON TrelEstProg.IntProgId = TblPrograma.IdPrograma
			WHERE IdsMatricId = @IdRelacion
		END

	ELSE IF @IdServicio = 91 --INSCRIPCION  
		BEGIN
			SELECT reg.IdRegistro, asp.IdAspirante, CONCAT(NombrePrimero, ' ', ApellidoPrimero, ' ',ApellidoOtros), DocumentoId, IdPeriodo 
			FROM Inscripciones.DBO.Aspirantes Asp
			      INNER JOIN Inscripciones.DBO.Registros Reg ON Reg.IdAspirante = Asp.IdAspirante
			WHERE IdRegistro = @IdRelacion						
		END



	ELSE IF @IdServicio = 850 --MATRICULAS CENTRO DE IDIOMAS
		BEGIN 
			SELECT Insc.idsInscripId, IdsTerceroId, ChrTerNomCompleto, grup.chrGrupNom, intTempo
			 FROM CIdiom.DBO.tblInscrip Insc
			INNER JOIN Contabilidad.DBO.TblTercero ON TblTercero.IdsTerceroId = Insc.intTerceroId
			INNER JOIN CIdiom.DBO.Inscripciones_Grupos InscGrup ON Insc.idsInscripId = InscGrup.IdInscripcion
			INNER JOIN CIdiom.DBO.tblGrupo Grup ON InscGrup.GrupoDeseado = Grup.idsGrupoId
			INNER JOIN CIdiom.DBO.tblTemp ON tblTemp.idsTempId = Grup.intTempo
			WHERE idsInscripId = @IdRelacion
		END

   
	ELSE IF @IdServicio = 819 -- SLIES
		BEGIN

			SELECT IdInscripcion, PER.IdPersona, ASIS.IdAsistente, ACT.IdActividad, act.IdEvento, CONCAT(per.Nombre1, ' ' ,per.Apellido1, ' ' ,per.Apellido2), Ins.Aprobada
			FROM SLIES2.dbo.Personas Per
			INNER JOIN SLIES2.dbo.Asistentes Asis ON Asis.IdPersona = Per.IdPersona
			INNER JOIN SLIES2.dbo.Inscripciones Ins ON Ins.IdAsistente =  Asis.IdAsistente
			inner join SLIES2.dbo.Actividades act on act.IdActividad = Ins.IdActividad
			WHERE IdInscripcion = @IdRelacion			 					  
								   
		END

