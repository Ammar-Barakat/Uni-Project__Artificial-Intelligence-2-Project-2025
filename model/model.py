from flask import Flask, request, jsonify
from flask_cors import CORS  # Import CORS
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.metrics.pairwise import cosine_similarity
import pandas as pd
import numpy as np

app = Flask(__name__)
CORS(app)  # Enable CORS for all routes

df = pd.read_csv('books.csv')

df = df[['bookId', 'title', 'author', 'description', 'language', 'genres', 'rating']]
df = df.fillna('')

df['features'] = df['author'] + ' ' + df['genres']

vectorizer = CountVectorizer(stop_words='english', min_df=20)
word_matrix = vectorizer.fit_transform(df['features'])

chunk_size = 5000
num_chunks = int(np.ceil(word_matrix.shape[0] / chunk_size))
chunks = [word_matrix[i * chunk_size:(i + 1) * chunk_size] for i in range(num_chunks)]

@app.route("/get-recommendations/<book_name>")
def get_recommendations(book_name):
    index = df.index[df['title'].str.lower() == book_name.lower()]

    if len(index) == 0:
        return jsonify([]), 404

    target_index = index[0]
    target_chunk_idx = target_index // chunk_size
    target_chunk_offset = target_index % chunk_size

    target_language = df.iloc[target_index]['language']

    within_chunk_sim = cosine_similarity(chunks[target_chunk_idx])
    similarities = list(enumerate(within_chunk_sim[target_chunk_offset]))

    for i, chunk in enumerate(chunks):
        if i == target_chunk_idx:
            continue
        inter_chunk_sim = cosine_similarity(chunks[target_chunk_idx][target_chunk_offset], chunk)
        similarities.extend([(j + i * chunk_size, inter_chunk_sim[0, j]) for j in range(chunk.shape[0])])

    recommendations = [
        (idx, sim_score, df.iloc[idx]['rating'], df.iloc[idx]['language'])
        for idx, sim_score in similarities
    ]

    recommendations = [rec for rec in recommendations if rec[3].lower() == target_language.lower()]

    recommendations = sorted(
        recommendations,
        key=lambda x: (x[1], x[2]),
        reverse=True
    )

    top_recs = recommendations[1:20 + 1]

    titles = [df.iloc[rec[0]]['bookId'] for rec in top_recs]

    return jsonify(titles), 200

if __name__ == "__main__":
    app.run(debug=True)
