from flask import Flask, request, jsonify
from flask_cors import CORS  
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import pandas as pd
import numpy as np

app = Flask(__name__)
CORS(app)  

df = pd.read_csv('goodreads_data.csv')

df = df[['Book', 'Author', 'Description', 'Genres', 'Avg_Rating']]
df = df.fillna('')
df = df.drop_duplicates(subset=["Book"])

df['features'] = df['Author'] + ' ' + df['Genres']

vectorizer = CountVectorizer(stop_words='english', min_df=20)
word_matrix = vectorizer.fit_transform(df['features'])

@app.route("/get-recommendations/<title>/<author>/<genres>")
def get_recommendations(title, author, genres, count=10):
    new_book_df = pd.DataFrame({"Title": [title], "Author": [author], "Genres": [genres]})

    new_book_df['features'] = new_book_df['Author'] + ' ' + new_book_df['Genres']

    new_book_df_vector = vectorizer.transform(new_book_df['features'])
    
    sim_scores = cosine_similarity(new_book_df_vector, word_matrix).flatten()

    sim_scores = list(enumerate(sim_scores))

    sim_scores = [score for score in sim_scores if df.iloc[score[0]]["Book"] != title]

    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)

    sim_indices = [i[0] for i in sim_scores[:count]]

    recommendations_with_ratings = df.iloc[sim_indices][["Book", "Avg_Rating"]]

    recommendations_with_ratings = recommendations_with_ratings.sort_values(by="Avg_Rating", ascending=False)

    return recommendations_with_ratings["Book"].tolist()

if __name__ == "__main__":
    app.run(debug=True)
