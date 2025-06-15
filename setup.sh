#!/bin/bash

echo "Checking project structure..."

# Create necessary directories if they don't exist
mkdir -p public

# Print the current directory structure
echo "Current directory structure:"
ls -la

# Check if the required files exist
echo "Checking for required files..."
for file in "App.jsx" "App.css" "index.js" "package.json" "public/index.html" "public/manifest.json" "components/FlipCard.jsx" "components/FlipCard.css" "components/CelebrityReaction.jsx" "components/CelebrityReaction.css" "services/reactionService.js"
do
  if [ -f "$file" ]; then
    echo "$file exists"
  else
    echo "$file is missing!"
  fi
done

# Install dependencies
echo "Installing dependencies..."
npm install

# Start the dev server
echo "Starting development server..."
npm start 