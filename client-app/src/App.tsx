import React, { useEffect, useState } from 'react';
import { useMsal, useIsAuthenticated } from '@azure/msal-react';
import { loginRequest } from './authConfig';

interface Product {
    name: string;
}


function App() {
    const { instance, inProgress } = useMsal();
    const isAuthenticated = useIsAuthenticated();
    const [products, setProducts] = useState<Product[]>([]);

    useEffect(() => {
        if (!isAuthenticated && inProgress === 'none') {
            instance.loginPopup(loginRequest).catch(e => {
                console.error(e);
            });
        }
    }, [isAuthenticated, inProgress, instance]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const account = instance.getAllAccounts()[0];
                if (!account) return;

                const response = await instance.acquireTokenSilent({
                    ...loginRequest,
                    account,
                });

                const token = response.accessToken;

                const apiResponse = await fetch('https://localhost:7062/api/products', {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                const data = await apiResponse.json();
                setProducts(data);
            } catch (err) {
                console.error('Error fetching products', err);
            }
        };

        if (isAuthenticated && inProgress === 'none') {
            fetchData();
        }
    }, [isAuthenticated, inProgress, instance]);

    return (
        <div>
            <h1>Products</h1>
            {products.map((p, i) => (
                <div key={i}>{p.name}</div>
            ))}
        </div>
    );
}

export default App;
