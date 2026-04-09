import fallbackImage from "@/public/default.jpg";

export const Image = ({ src, alt, className, ...rest }) => {
  const imageUrl = src
    ? "http://127.0.0.1:10000/devstoreaccount1/media/uploads/" + src
    : fallbackImage;
  return <img src={imageUrl} alt={alt} className={className} {...rest} />;
};
