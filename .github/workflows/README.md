# Deploying Zodiac

One supplementary aim of this project is to understand deployment with Github actions a lot better.  I'm far from an expert - so dont think the patterns I have used here are best proactice, they almost certainly are not.
However, one spin off benefit is that you should be able to deploy the whole Zodiac service (and its consituent microsoervices) very easily indeed.

## Secrets

Secrets are environment variables that are encrypted and only exposed to selected actions. Anyone with collaborator access to this repository can use these secrets in a workflow.

Secrets are not passed to workflows that are triggered by a pull request from a fork. [Learn more](https://help.github.com/actions/automating-your-workflow-with-github-actions/creating-and-using-encrypted-secrets).

In order to deploy zodiac you will need to define two secrets in you project:
##### AZURE_CREDENTIALS

AZURE_CREDENTIALS are used by github as an identity to access your Azure subscription where Zodiac will be deployed (for those familiar with Azure DevOps, its very similar in funciton to a Service Connection.  Instructions for creating AZURE_CREDENTIALS are [here](https://github.com/Azure/CLI#configure-azure-credentials-as-github-secret)

##### DB_ADMIN_PASSWORD

The DB_ADMIN_PASSWORD will be used in connectin strings and if you'd like to access the sample azure sql database from the Azure portal.
It can be any valid password for Azure SQL Database.

![nicks secrets](/docs/nicks-secrets.jpg)

## Workflows
## Deployment scripts
