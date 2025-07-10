import { AccountInfo, IPublicClientApplication } from "@azure/msal-browser";
import { loginRequest } from "../authConfig";

const API_URL = "https://localhost:7062/api/products";

export const fetchProducts = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo
): Promise<any[]> => {
    const response = await msalInstance.acquireTokenSilent({
        ...loginRequest,
        account,
    });

    const token = response.accessToken;

    const res = await fetch(API_URL, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    if (!res.ok) {
        throw new Error("Failed to fetch products");
    }

    return await res.json();
};
