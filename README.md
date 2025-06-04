# BasicMatch3Solitaire

# What I Built
This is a basic card and stack-based Match-3 game, inspired by solitaire mechanics. The project includes:

A random level generator

A simple level editor

A full game loop that allows continuous play

Implement the Command Pattern thoroughly for all game actions, improving undo/redo functionality and making the game logic more robust and extendable.

While there isn’t currently a formal fail condition, one idea was to trigger a fail state if any stack exceeds a certain size.

# What I’d Improve with More Time
Given more time, I would focus on improving the scalability and maintainability of the codebase. Some specific improvements:

Move the Card logic to a State Pattern to better manage different card states.

Use the Strategy Pattern for movement behaviors, which would make it easier for the product team to tweak interactions without deep changes in the code.

I would avoid writing a custom editor in a tight time constraint, as it consumed a significant portion of the development time.

# AI Assistance
AI tools were used in the following ways:

At the beginning, I used AI to understand how Solitaire-like mechanics typically work, since I wasn’t familiar with them and never played Solitaire(yep thats odd)

Throughout development, I had GitHub Copilot running in my IDE, which helped with code completion and suggestions.

Additionally, AI (CHATGPT) was used to help with formatting and structuring the README content for clarity.
