import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import IndexPage from './components/IndexPage';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <IndexPage />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

// src/components/Index.js

// import React, { useState } from 'react';
// import QuestionPage from './components/Question';
// import CardPage from './components/Card';
// import Ollama from "ollama";





// const Index = () => {
//   const [userQuestion, setUserQuestion] = useState('');
//   const [celebrityData, setCelebrityData] = useState({
//     name: 'John Doe', // Default celebrity name
//     image: 'https://via.placeholder.com/150', // Default celebrity image URL
//   });

//   const handleQuestionSubmit = async (question) => {
//     // Assuming you have an API endpoint to get celebrity data
//     // Replace this with your actual API call

//     const response = await Ollama.chat({
//       model: 'llama2',
//       messages: [{ role: 'user', content: 'How would Alan Turing reply to a tweet which says Code with Passion and Strive for Excellence?'}]
//     });
    
//     console.log(response.message.content);

//     // fetchCelebrityData(question);
//   };

//   const fetchCelebrityData = async (question) => {
//     try {
//       // Make an API call to get celebrity data based on user's question
//       // Example response:
//       const response = await fetch('https://api.example.com/celebrity', {
//         method: 'POST',
//         body: JSON.stringify({ question }),
//         headers: {
//           'Content-Type': 'application/json',
//         },
//       });
//       const data = await response.json();

//       // Update celebrity data
//       setCelebrityData({
//         name: data.name,
//         image: data.image,
//       });
//     } catch (error) {
//       console.error('Error fetching celebrity data:', error);
//     }
//   };

//   const flipCard = () => {
//     // Implement card flip logic here
//     // You can use CSS transitions or libraries like react-card-flip
//     // Toggle the display of impersonated response on the card back
//   };

//   const root = ReactDOM.createRoot(document.getElementById('root'));
//   root.render(
//     <React.StrictMode>
//       <QuestionPage onQuestionSubmit={handleQuestionSubmit} />
//       <CardPage
//             celebrityName={celebrityData.name}
//             celebrityImage={celebrityData.image}
//             flipCard={flipCard}
//           />
//     </React.StrictMode>
//   );  
// };

// export default Index;
