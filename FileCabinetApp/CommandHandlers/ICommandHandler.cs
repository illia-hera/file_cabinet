using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface <c>ICommandHandler</c>.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets the next.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <returns>Return new StatCommandHandler.</returns>
        ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="appCommandRequest">The command request.</param>
        void Handle(AppCommandRequest appCommandRequest);
    }
}
