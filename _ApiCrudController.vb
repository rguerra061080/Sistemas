Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Reflection
Imports CadastroMedicos.UI.Common
Imports CadastroMedicos.Domain
Imports CadastroMedicos.Data
Imports CadastroMedicos.Data.Repositories.Generic

Namespace Controllers.api

    Public MustInherit Class ApiCrudController(Of TDtoObject As DataTransferObject, TDataEntity As Class)
        Inherits ApiController

        Protected unitOfWork As IUnitOfWork

        Public Sub New()
            unitOfWork = New UnitOfWork()
        End Sub

        Public Sub New(unitOfWork As IUnitOfWork)
            MyClass.unitOfWork = unitOfWork
        End Sub

        Protected Function CreateDtoInstance(dataEntity As TDataEntity) As TDtoObject
            Dim dto As TDtoObject

            dto = Activator.CreateInstance(GetType(TDtoObject)) 'New TDtoObject

            Utils.TransferDataFromObject(dataEntity, dto)
            Return dto
        End Function

        Protected MustOverride Function GetRepository() As IRepository(Of TDataEntity)

        <HttpGet>
        Public Function GetAll() As IHttpActionResult

            Dim repository As IRepository(Of TDataEntity) = GetRepository()

            Dim resultType As Func(Of TDataEntity, TDtoObject)
            Dim allData As IList(Of TDataEntity)
            Dim dtoResult As IList(Of TDtoObject)

            allData = repository.GetAll().ToList()

            resultType = Function(dataEntity As TDataEntity)
                             Return CreateDtoInstance(dataEntity)
                         End Function

            dtoResult = allData.Select(resultType).ToList()
            Return Ok(dtoResult)
        End Function

        <HttpGet>
        Public Function GetById(id As Long) As TDtoObject

            Dim dto As TDtoObject
            Dim dataEntity As TDataEntity

            dto = Activator.CreateInstance(GetType(TDtoObject)) 'New TDtoObject

            dataEntity = GetRepository().GetById(id)
            If Not IsNothing(dataEntity) Then
                Utils.TransferDataFromObject(dataEntity, dto)
            End If

            Return dto
        End Function

        <HttpPost>
        Public Function Post(dto As TDtoObject) As IHttpActionResult

            Dim dataEntity As TDataEntity = Activator.CreateInstance(GetType(TDataEntity))
            Dim repository As IRepository(Of TDataEntity) = GetRepository()
            Try
                Utils.TransferDataFromObject(dto, dataEntity)
                repository.Insert(dataEntity)
                unitOfWork.SaveChanges()
                Return Ok("Registro incluído com sucesso.")
            Catch ex As Exception
                Return BadRequest(String.Format("Erro ao processar a solicitação: {0}", ex.Message))
            End Try

        End Function

        <HttpPut>
        Public Function Put(id As Integer, dto As TDtoObject) As IHttpActionResult

            Dim dataEntity As TDataEntity
            Dim repository As IRepository(Of TDataEntity)

            Try
                repository = GetRepository()
                dataEntity = repository.GetById(dto.Id)

                If dataEntity Is Nothing Then
                    Return BadRequest("Registro não encontrado")
                End If

                Utils.TransferDataFromObject(dto, dataEntity)
                repository.Update(dataEntity)

                unitOfWork.SaveChanges()

                Return Ok("Registro atualizado com sucesso.")
            Catch ex As Exception
                Return BadRequest(String.Format("Erro ao processar a solicitação: {0}", ex.Message))
            End Try
        End Function


        <HttpDelete>
        Public Function Delete(id As Integer) As IHttpActionResult

            Dim dataEntity As TDataEntity
            Dim repository As IRepository(Of TDataEntity)

            Try
                repository = GetRepository()
                dataEntity = repository.GetById(id)

                If dataEntity Is Nothing Then
                    Return BadRequest("Registro não encontrado")
                End If

                repository.Delete(dataEntity)
                unitOfWork.SaveChanges()

                Return Ok("Registro excluído com sucesso.")
            Catch ex As Exception
                Return BadRequest(String.Format("Erro ao processar a solicitação: {0}", ex.Message))
            End Try

        End Function

    End Class

End Namespace