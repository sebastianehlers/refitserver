using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace RefitServer.Generator.Extensions;

public static class NamespaceExtractor
{
    public static string GetUsingsDeclarationString(this INamedTypeSymbol namedType)
    {
        var requiredNamespaces = new HashSet<string>();

        // Base type's namespace
        if (namedType.BaseType != null &&
            namedType.BaseType.ContainingNamespace != null &&
            !namedType.BaseType.ContainingNamespace.IsGlobalNamespace)
        {
            requiredNamespaces.Add(namedType.BaseType.ContainingNamespace.ToString());
        }

        // Interfaces' namespaces
        foreach (var interfaceType in namedType.Interfaces)
        {
            if (!interfaceType.ContainingNamespace.IsGlobalNamespace)
            {
                requiredNamespaces.Add(interfaceType.ContainingNamespace.ToString());
            }
        }

        // Members' (methods, properties) types' namespaces
        foreach (var member in namedType.GetMembers())
        {
            switch (member)
            {
                case IMethodSymbol methodSymbol:
                    AddNamespace(methodSymbol.ReturnType);
                    foreach (var parameter in methodSymbol.Parameters)
                    {
                        AddNamespace(parameter.Type);
                    }
                    break;
                case IPropertySymbol propertySymbol:
                    AddNamespace(propertySymbol.Type);
                    break;
                case IFieldSymbol fieldSymbol:
                    AddNamespace(fieldSymbol.Type);
                    break;
            }
        }

        // Local function to add namespace if it's not the global namespace or the same as the type's namespace
        void AddNamespace(ITypeSymbol typeSymbol)
        {
            // Recursively add namespaces for generic type arguments
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    AddNamespace(typeArgument); // Recursive call for each type argument
                }
            }

            // Add the namespace of the type symbol itself if it's not global and not the same as the class's namespace
            if (!typeSymbol.ContainingNamespace.IsGlobalNamespace && 
                typeSymbol.ContainingNamespace.ToString() != namedType.ContainingNamespace.ToString())
            {
                requiredNamespaces.Add(typeSymbol.ContainingNamespace.ToString());
            }
        }

        // Concatenate all required namespaces into a single string with using directives
        return string.Join("\n", requiredNamespaces.Select(ns => $"using {ns};"));
    }
    
    public static string GetNamespaceDeclarationString(this INamedTypeSymbol namedType)
    {
        // Initialize an empty list to hold namespace parts
        var namespaceParts = new List<string>();
    
        // Get the containing namespace of the named type
        var namespaceSymbol = namedType.ContainingNamespace;
    
        // Loop through all containing namespaces up to the root namespace
        while (namespaceSymbol != null && !namespaceSymbol.IsGlobalNamespace)
        {
            // Insert the name of the current namespace at the beginning of the list
            // because we're moving from the innermost namespace outwards
            namespaceParts.Insert(0, namespaceSymbol.Name);
        
            // Move to the next outer namespace
            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }
    
        // Join the namespace parts with '.' to form the full namespace declaration
        var fullNamespace = string.Join(".", namespaceParts);
    
        return $"namespace {fullNamespace};";
    }
}
