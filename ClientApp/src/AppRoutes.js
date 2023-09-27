import {Home} from "./components/Home";
import BookPage from "./components/BookPage/BookPage.tsx";

const AppRoutes = [
    {
        index: true,
        element: <Home/>
    },
    {
        path: '/books',
        element: <BookPage/>
    }
];

export default AppRoutes;
