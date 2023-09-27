import {useEffect, useState} from 'react';
import {Modal} from "reactstrap";
// @ts-ignore
import ListBooks from "./ListBooks.tsx";
// @ts-ignore
import CreateBook from "./CreateBook.tsx";
// @ts-ignore
import UpdateBook from "./UpdateBook.tsx";
// @ts-ignore
import {Book} from "./Models/Book.tsx";
// @ts-ignore
import {Genre} from "./Enums/Genre.tsx";

const apiUrl = 'https://localhost:5000/api';

export default function BookPage() {
    const defaultBookArrayState: Book[] = [];
    const defaultBookState: Book = {
        isbn: "",
        isbn13: "",
        title: "",
        author: "",
        yearPublished: 0,
        description: "",
        genre: Genre.Other
    };
    const [books, setBooks] = useState(defaultBookArrayState);
    const [loading, setLoading] = useState(true);
    const [needToLoad, setNeedToLoad] = useState(true);
    const [showCreateBookModal, setShowCreateBookModal] = useState(false);
    const [showUpdateBookModal, setShowUpdateBookModal] = useState(false);
    const [selectedBook, setSelectedBook] = useState<Book>(defaultBookState);

    async function fetchBookData() {
        setLoading(true);
        const response = await fetch(`${apiUrl}/books`);
        const data = await response.json();
        setBooks(data);
        setLoading(false);
    }

    function reloadBookData() {
        setNeedToLoad(true);
    }

    function clearModals() {
        setShowCreateBookModal(false);
        setShowUpdateBookModal(false);
        setSelectedBook(defaultBookState);
        reloadBookData();
    }

    function handleUpdateBook(book: Book) {
        setSelectedBook(book);
        setShowUpdateBookModal(true);
    }

    function handleDeleteBook(book: Book) {
        const bookId = book.isbn13 ? book.isbn13 : book.isbn;

        fetch(`${apiUrl}/books/${bookId}`, {
            method: 'DELETE',
            headers: {'Content-Type': 'application/json'},
        })
            .then(() => {
                    reloadBookData();
                }
            );
    }

    useEffect(() => {
        if (needToLoad) {
            fetchBookData();
            setNeedToLoad(false);
        }
    }, [books, needToLoad, showCreateBookModal, showUpdateBookModal]);

    if (loading && !books) {
        return <p>Loading...</p>
    }

    return (
        <div>
            <h1 id="tabelLabel">Books</h1>
            <button onClick={() => setShowCreateBookModal(true)}>Create</button>
            <button onClick={() => reloadBookData()}>Refresh</button>
            <ListBooks books={books} handleUpdateBook={handleUpdateBook} handleDeleteBook={handleDeleteBook}/>

            <Modal isOpen={showUpdateBookModal} toggle={() => setShowUpdateBookModal(false)}>
                <UpdateBook handleCancel={clearModals} book={selectedBook}/>
            </Modal>
            <Modal isOpen={showCreateBookModal} toggle={() => setShowCreateBookModal(false)}>
                <CreateBook handleCancel={clearModals}/>
            </Modal>
        </div>
    );
}
        