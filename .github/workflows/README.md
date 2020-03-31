# Deploying Zodiac

One supplementary aim of this project is to understand deployment with Github actions a lot better.  I'm far from an expert - so dont think the patterns I have used here are best proactice, they almost certainly are not.
However, one spin off benefit is that you should be able to deploy the whole Zodiac service (and its consituent microsoervices) very easily indeed.

## Secrets

Secrets are environment variables that are encrypted and only exposed to selected actions. Anyone with collaborator access to this repository can use these secrets in a workflow.

Secrets are not passed to workflows that are triggered by a pull request from a fork. [Learn more](https://help.github.com/actions/automating-your-workflow-with-github-actions/creating-and-using-encrypted-secrets).

In order to deploy zodiac you will need to define two secrets in you project:
### azure_credentials

AZURE_CREDENTIALS are used by github as an identity to access your Azure subscription where Zodiac will be deployed (for those familiar with Azure DevOps, its very similar in funciton to a Service Connection.  Instructions for creating AZURE_CREDENTIALS are [here](https://github.com/Azure/CLI#configure-azure-credentials-as-github-secret)

### db_admin_password

The DB_ADMIN_PASSWORD will be used in connectin strings and if you'd like to access the sample azure sql database from the Azure portal.
It can be any valid password for Azure SQL Database.

![nicks secrets](/docs/nicks-secrets.jpg)

## Workflows
[Github Actions](https://help.github.com/en/actions/getting-started-with-github-actions/about-github-actions) help you automate your software development workflows in the same place you store code and collaborate on pull requests and issues. You can write individual tasks, called actions, and combine them to create a custom workflow. Workflows are custom automated processes that you can set up in your repository to build, test, package, release, or deploy any code project on GitHub.

Zodiac has two sorts of workflow (this distinction is mine):

### Infrastructure
The infrastructure deployment workflow creates all the necessary infrastructure on which to deploy the application.  You can [see the workflow here](/.github/workflows/deploy_infrastructure.yaml).  This workflow is little more than a shell.  It has a trigger so that any change to the path **/global** will trigger the workflow to run.  Since you dont want the infrastrucutre to be updated every time you check in it makes sense to just edit the trigger.md file in global when you want to trigger it.

I said it was just a shell.  All it does is check out the zodiac repo onto the build agent, ensure the deployment scripts are executable, and run the master script [deploy_infrastructure.sh](/deployment_scripts/deploy_infrastructure.sh).  The [deployment scripts](/deployment_scripts/README.md) are stored together in a separate folder.

### Application Deployment

## Deployment scripts
