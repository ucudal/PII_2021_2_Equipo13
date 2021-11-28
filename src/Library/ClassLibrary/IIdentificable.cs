namespace PII_E13
{
    /// <summary>
    /// Interfaz que representa a cualquier tipo identificable a través de una cadena de caracteres única.
    /// </summary>
    public interface IIdentificable
    {
        /// <summary>
        /// Identificador único del objeto.
        /// </summary>
        /// <value>Cadena de caracteres conteniendo al identificador único del objeto.</value>
        string Id { get; }
    }
}