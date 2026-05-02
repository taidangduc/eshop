import { useEffect } from "react";
import { useAuth } from "./context";
import { API_BASE_URL } from "@env";

/*
 * AuthGuard is a component that checks if the user is authenticated before rendering the children.
 * If the user is not authenticated, it redirects them to the login page.
 */
export const AuthGuard = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();

  useEffect(() => {
    if (!isLoading && !isAuthenticated) {
      const returnUrl = encodeURIComponent(
        window.location.pathname + window.location.search,
      );

      window.location.href = `${API_BASE_URL}/login?returnUrl=${returnUrl}`;
    }
  }, [isAuthenticated, isLoading]);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated) {
    return null;
  }

  return children;
};
