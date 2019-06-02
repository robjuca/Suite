*Projeto*: ![](Docs/media/Suite.png) Suite 

![](Docs/media/73b1518089c9bf2b6b6d0dec5b38286f.jpg)

![](Docs/media/48edbf6725eabd4e44f7351b30205604.png)

*Objetivo*:

Produzir uma lista vertical de gadgets (documento, imagem, vídeo etc.)
arranjados em uma lista horizontal e salvar o resultado em um banco de dados. A
ideia surgiu do site da Microsoft (figura abaixo)
![](Docs/media/1dfbcb5311ca040aef14ec476ce203ba.jpg)

Criar aplicativos (WPF) para gerenciar gadgets e fazer o layout dos mesmos
salvando o resultado em um banco de dados (SQL Server) O projeto pode ser
dividido como: (figura abaixo)

| launcher | gadget     | layout                    |
|----------|------------|---------------------------|
| main app | documento  | bag                       |
|          | imagem     | horizontal: shelf, drawer |
|          | vídeo etc. | vertical: chest           |

![](Docs/media/c6a66df256daf27eb3648ed684493f9e.jpg)

*Layout*:

Os gadgets são formatados para uma matriz (dashboard) de 4cx4r dando um total de
16 possíveis layouts (tamanhos). Para identificar o size de uma cell neste
dashboard

usa-se “style”.

| Style               |                |
|---------------------|----------------|
| horizontal (column) | Vertical (row) |
| mini                | mini           |
| small               | small          |
| large               | large          |
| big                 | big            |

![](Docs/media/b1ce885d52718db50b27de09967fbb05.jpg)

*Settings*:

\- tamanho (pixel) da coluna

\- connection string SQL Server

*Ferramentas* (tools):

| IDE                | Language   | Tools                                       | UI                       |
|--------------------|------------|---------------------------------------------|--------------------------|
| Visual Studio 2019 | C\# 8      | Caliburn.Micro 3.2 (MVVM pattern)           | MaterialDesignThemes 2.5 |
|                    | .Net 4.8   | XDMessaging.Lite 5                          | gong-wpf-dragdrop 1.1    |
|                    |            | Microsoft.EntityFrameworkCore.SqlServer 2.2 |                          |
|                    |            | rrLibrary                                   |                          |

![](Docs/media/SuiteGadgetDocument.jpg)
![](Docs/media/SuiteGadgetImage.jpg)
![](Docs/media/SuiteLayoutBag.jpg)
![](Docs/media/SuiteLayoutShelf.jpg)
![](Docs/media/SuiteLayoutDrawer.jpg)
![](Docs/media/SuiteLayoutChest.jpg)

*Database access* (flowchart):

![](Docs/media/DBFlowchart.jpg)

*SQL Database* (table models):

![](Docs/media/ModelComponent.jpg)
![](Docs/media/ModelExtension.jpg)
![](Docs/media/ModelSettings.jpg)

*SQL Database* (Server Context Engine):

![](Docs/media/ServerContextEngine.jpg)

*SQL Database* (Server Services):

![](Docs/media/ServerServicesComponent.jpg)

*Entity Action* (Model Engine):

![](Docs/media/EntityAction.jpg)

*Suite Shared* :

![](Docs/media/SuiteSharedResources.jpg)
![](Docs/media/SuiteSharedTypes.jpg)
![](Docs/media/SuiteSharedServices.jpg)
![](Docs/media/SuiteSharedMessage.jpg)
![](Docs/media/SuiteSharedDashboard.jpg)
![](Docs/media/SuiteSharedViewModel.jpg)

*Suite Shared Gadget* :
![](Docs/media/SuiteSharedGadget.jpg)

*Suite Shared Layout* :
![](Docs/media/SuiteSharedLayout.jpg)

*Suite Module Settings* :
![](Docs/media/SuiteModuleSettings.jpg)

*Suite Launcher* : (application entry point)
![](Docs/media/SuiteLauncherApplication.jpg)

*Suite Gadget* : (application)
![](Docs/media/SuiteGadgetDocumentApp.jpg)
![](Docs/media/SuiteGadgetImageApp.jpg)

*Suite Layout* : (application)
![](Docs/media/SuiteLayoutBagApp.jpg)
![](Docs/media/SuiteLayoutShelfApp.jpg)
![](Docs/media/SuiteLayoutDrawerApp.jpg)
![](Docs/media/SuiteLayoutChestApp.jpg)

*Suite Solution* : (application)
![](Docs/media/SuiteApplicationSolution.jpg)

*Suite Server Solution* : (library)
![](Docs/media/SuiteServerSolution.jpg)

*Suite Shared Solution* : (library)
![](Docs/media/SuiteSharedSolution.jpg)