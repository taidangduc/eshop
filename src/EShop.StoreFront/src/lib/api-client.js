import axios, { AxiosError } from "axios";
import { API_BASE_URL } from "@env";

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1",
  },
});