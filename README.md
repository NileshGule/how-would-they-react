# How Would They React?

A React application that generates AI-powered celebrity reactions to various scenarios.

## Description

"How Would They React?" is an interactive web application that allows users to enter scenarios and see how different celebrities might react to them. The application uses a flip card UI to display the celebrity image on the front and their AI-generated reaction on the back.

## Features

- Enter custom scenarios
- Filter celebrities by categories (Cricketers, Sports Persons, Politicians, etc.)
- Interactive flip card display
- Mock API with pre-generated celebrity responses

## Project Structure

```
how-would-they-react/
├── App.jsx                  # Main application component
├── App.css                  # Main application styles
├── index.js                 # Application entry point
├── package.json             # Project dependencies and scripts
├── public/                  # Public assets
│   ├── index.html           # HTML template
│   └── manifest.json        # Web app manifest
├── components/              # React components
│   ├── CelebrityReaction.jsx # Celebrity selection and filtering component
│   ├── CelebrityReaction.css # Styles for celebrity selection
│   ├── FlipCard.jsx         # Flip card component for displaying reactions
│   └── FlipCard.css         # Styles for flip card
└── services/                # API and data services
    └── reactionService.js   # Mock API for celebrity data and reactions
```

## How to Build and Run

### Prerequisites

- Node.js (v14 or later)
- npm (v6 or later)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/how-would-they-react.git
   cd how-would-they-react
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open your browser and navigate to http://localhost:3000

## Using the Application

1. Enter a scenario in the text area (e.g., "Finding $100 on the sidewalk")
2. Click "Get Reactions" to proceed
3. Select a category to filter celebrities
4. Click on a celebrity to see their reaction
5. Click on the card to flip it and see the AI-generated reaction
6. Click "Try a New Scenario" to start over

## Note for Development

This application currently uses mock data for demonstration purposes. In a production environment, you would integrate with a real AI service to generate the reactions.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Related Project: How Would They React .NET Console

This repository also contains a .NET console application that generates AI-powered celebrity reactions using LLMs and Microsoft.AI.Foundry.Local. You can find it in the [`how-would-they-react-dotnet`](./how-would-they-react-dotnet/README.md) folder.

- [How Would They React .NET Console - Setup and Usage](./how-would-they-react-dotnet/README.md)
